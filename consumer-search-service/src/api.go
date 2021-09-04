package main

import (
	"net/http"

	"cdc-testing-workshop/consumer-search-service/src/dto"
	"cdc-testing-workshop/consumer-search-service/src/httpclient"

	"github.com/gin-gonic/gin"
)

func searchProducts(context *gin.Context) {
	client := httpclient.NewProductServiceClient()
	keyword,_ := context.GetQuery("keyword")

	products, err := client.SearchProducts(keyword)
	if err != nil {
		context.JSON(http.StatusBadRequest, err.Error())
		return
	}

	result := make([]dto.ProductSearchDto, 0, len(products))
	for _, product := range products {
		result = append(result, dto.ProductSearchDto{
			ID:   product.ID,
			Name: product.Name,
		})
	}
	context.JSON(http.StatusOK, result)
}
