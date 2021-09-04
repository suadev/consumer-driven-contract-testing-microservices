package test

import (
	"cdc-testing-workshop/consumer-price-service/src/dto"
	"cdc-testing-workshop/consumer-price-service/src/httpclient"
	"fmt"
	"log"
	"os"
	"path/filepath"
	"testing"
	"time"

	"github.com/pact-foundation/pact-go/dsl"
	"github.com/pact-foundation/pact-go/types"
)

var pact dsl.Pact

func TestMain(m *testing.M) {

	setupMockServer()

	var exitCode = m.Run()

	pact.WritePact()
	pact.Teardown()

	err := publishPacts()
	if err != nil {
		log.Println("ERROR: ", err)
		os.Exit(1)
	}
	os.Exit(exitCode)
}

func publishPacts() error {

	publisher := dsl.Publisher{}

	var dir, _ = os.Getwd()
	var pactDir = fmt.Sprintf("%s/../../_pacts", dir)

	now := time.Now()
	consumerVersion := fmt.Sprintf("1.0.%02d.%02d.%02d.%02d", now.Day(), now.Month(), now.Hour(), now.Minute())

	return publisher.Publish(types.PublishRequest{
		PactURLs:        []string{filepath.FromSlash(fmt.Sprintf("%s/price-service-product-service.json", pactDir))},
		ConsumerVersion: consumerVersion,
		PactBroker:      "http://localhost:9292",
		BrokerUsername:  "admin",
		BrokerPassword:  "admin",
	})
}

var term = dsl.Term

type request = dsl.Request

func Test_Create_Get_Products_Contract(t *testing.T) {
	t.Run("Get Products...", func(t *testing.T) {
		productsResponse := [1]dto.ProductDto{
			{
				ID:       1,
				Name:     "Lorem Ipsum",
				IsActive: true,
			}}

		pact.
			AddInteraction().
			Given("There are available products").
			UponReceiving("A GET request to retrieve products.").
			WithRequest(request{
				Method: "GET",
				Path:   term("/products", "/products"),
			}).
			WillRespondWith(dsl.Response{
				Status: 200,
				Body:   dsl.Like(productsResponse),
				Headers: dsl.MapMatcher{
					"Content-Type": term("application/json; charset=utf-8", `application\/json`),
				},
			})

		err := pact.Verify(func() error {
			_, err := productServiceClient.GetProducts()
			return err
		})
		if err != nil {
			t.Fatalf("Error on Verify: %v", err)
		}
	})
}

func Test_Create_Get_Single_Product_Contract(t *testing.T) {
	t.Run("Get Product...", func(t *testing.T) {
		productResponse := dto.ProductDto{
			ID:       1,
			Name:     "Lorem Ipsum",
			IsActive: true,
		}

		pact.
			AddInteraction().
			Given("There is a single product for a given product id").
			UponReceiving("A GET request to retrieve single product.").
			WithRequest(request{
				Method: "GET",
				Path:   term("/products/1", "/products/[0-9]+"),
			}).
			WillRespondWith(dsl.Response{
				Status: 200,
				Body:   dsl.Like(productResponse),
				Headers: dsl.MapMatcher{
					"Content-Type": term("application/json; charset=utf-8", `application\/json`),
				},
			})

		err := pact.Verify(func() error {
			_, err := productServiceClient.GetProduct(1)
			return err
		})
		if err != nil {
			t.Fatalf("Error on Verify: %v", err)
		}
	})
}

var productServiceClient *httpclient.ProductServiceClient

func setupMockServer() {
	pact = createPact()
	// Start service to get access to the port
	pact.Setup(true)
	productServiceClient = &httpclient.ProductServiceClient{
		HostUrl: fmt.Sprintf("http://localhost:%d", pact.Server.Port),
	}
}

func createPact() dsl.Pact {
	return dsl.Pact{
		Consumer:                 "price-service",
		Provider:                 "product-service",
		LogDir:                   "../_pact_consumer_logs",
		PactDir:                  "../../_pacts",
		LogLevel:                 "DEBUG",
		DisableToolValidityCheck: true,
	}
}
