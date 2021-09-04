package httpclient

import (
	"cdc-testing-workshop/consumer-search-service/src/dto"
	"encoding/json"
	"errors"
	"io/ioutil"
	"net/http"
)

type ProductServiceClient struct {
	HostUrl string
}

func NewProductServiceClient() *ProductServiceClient {
	return &ProductServiceClient{
		HostUrl: "http://localhost:3001/api",
	}
}

func (c *ProductServiceClient) SearchProducts(keyword string) ([]dto.ProductDto, error) {
	resp, err := http.Get(c.HostUrl + "/products/search?keyword=" + keyword)
	if err != nil {
		return nil, err
	}

	defer resp.Body.Close()
	if resp.StatusCode != 200 {
		return nil, errors.New("failed getting products")
	}

	body, err := ioutil.ReadAll(resp.Body)
	if err != nil {
		return nil, err
	}

	var products []dto.ProductDto
	json.Unmarshal(body, &products)
	return products, nil
}
