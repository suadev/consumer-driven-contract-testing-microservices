package main

import (
	"net/http"

	"github.com/gin-gonic/gin"
)

func main() {
	handler := initRoutes()
	err := newHttpServer(handler).ListenAndServe()
	if err != nil {
		panic("Couldn't start the HTTP server.")
	}
}

func newHttpServer(handler http.Handler) *http.Server {
	return &http.Server{
		Addr:    ":3002",
		Handler: handler,
	}
}

func initRoutes() *gin.Engine {
	router := gin.Default()
	routerGroup := router.Group("/api")
	routerGroup.GET("/product-prices", getProductPrices)
	routerGroup.GET("/product-price/:id", getProductPrice)
	return router
}
