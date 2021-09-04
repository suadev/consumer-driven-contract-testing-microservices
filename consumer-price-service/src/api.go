package main

import (
	"math/rand"
	"net/http"
	"strconv"

	"cdc-testing-workshop/consumer-price-service/src/dto"
	"cdc-testing-workshop/consumer-price-service/src/httpclient"

	"github.com/gin-gonic/gin"
)

func getProductPrices(context *gin.Context) {
	client := httpclient.NewProductServiceClient()
	products, err := client.GetProducts()
	if err != nil {
		context.JSON(http.StatusBadRequest, err.Error())
		return
	}

	result := make([]dto.ProductPriceDto, 0, len(products))
	for _, product := range products {
		result = append(result, dto.ProductPriceDto{
			ID:       product.ID,
			Name:     product.Name,
			IsActive: product.IsActive,
			Price:    100 + rand.Float64()*(500-100), // random price between 100-500
		})
	}
	context.JSON(http.StatusOK, result)
}

func getProductPrice(context *gin.Context) {
	productID, _ := strconv.Atoi(context.Param("id"))

	client := httpclient.NewProductServiceClient()
	product, err := client.GetProduct(productID)
	if err != nil {
		context.JSON(http.StatusBadRequest, err.Error())
		return
	}

	result := dto.ProductPriceDto{
		ID:       product.ID,
		Name:     product.Name,
		IsActive: product.IsActive,
		Price:    100 + rand.Float64()*(500-100), // random price between 100-500
	}
	context.JSON(http.StatusOK, result)
}
