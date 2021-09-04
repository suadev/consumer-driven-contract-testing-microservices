package httpclient

import (
	"cdc-testing-workshop/consumer-price-service/src/dto"
	"encoding/json"
	"errors"
	"fmt"
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

func (c *ProductServiceClient) GetProducts() ([]dto.ProductDto, error) {
	resp, err := http.Get(c.HostUrl + "/products")
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

func (c *ProductServiceClient) GetProduct(id int) (dto.ProductDto, error) {
	resp, err := http.Get(fmt.Sprintf("%s/products/%d", c.HostUrl, id))
	if err != nil {
		return dto.ProductDto{}, err
	}

	defer resp.Body.Close()
	if resp.StatusCode != 200 {
		return dto.ProductDto{}, errors.New("failed getting single product")
	}

	body, err := ioutil.ReadAll(resp.Body)
	if err != nil {
		return dto.ProductDto{}, err
	}

	var product dto.ProductDto
	json.Unmarshal(body, &product)
	return product, nil
}
