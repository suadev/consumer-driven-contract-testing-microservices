## Why?

E2E testing is one of the biggest challenges in distributed systems. Consumer-Driven Contract Testing is an excellent alternative to deal with this challenge.


This workshop shows how to implement Consumer-Driven Contract Testing between the microservices which are developed in different tech stacks.

I used <a href="https://github.com/pact-foundation">Pact</a> framework to create and verify contracts between the microservices. 

There are 3 microservices that communicate syncronously each other. 

**Porovider Service**   -> Product Service (.Net5)

**Consumer Service(1)** -> Search Service  (Go 1.16)

**Consumer Service(2)** -> Price Service   (Go 1.16)

## Run in Debug Mode 

You need Pact Broker and Postgres container to play workshop. So, run ```docker-compose up``` in root path.

Select 'All' Debug Configuration and start debugging. (for Vs Code users) All microservices will be running in debug mode. 

<img src="https://github.com/suadev/cdc-testing-with-pact-workshop/blob/main/_img/debugging.png" height=300, width=350></img>

You may want to play with the services before creating and verifying contracts. See sample postman requests <a href="https://github.com/suadev/cdc-testing-with-pact-workshop/blob/main/_postman/CDC_Testing.postman_collection.json">here.</a>


## Creating Contracts

After running all services, you need to publish your consumer-driven contracts. Run contract tests for Search and Price services;

``` 
cd consumer-price-service/test
go test
```

``` 
cd consumer-search-service/test
go test
``` 

After running these tests successfully, you will see the unverified contracts in Pact Broker home page which is hosting on http://localhost:9292

<img src="https://github.com/suadev/cdc-testing-with-pact-workshop/blob/main/_img/contracts.png"></img>

And, you can also see the network graph between the services by clicking on any of them.

<img src="https://github.com/suadev/cdc-testing-with-pact-workshop/blob/main/_img/graph.png"></img>


## Verifying Contracts

You have 2 contracts that published by 2 consumer services. To verify these contract, just run provider contract test like below.

``` 
cd provider-product-service/test
dotnet test
``` 

If the test is succeded, you need to see you verified contracts like below. Pay attention to "Last Verified" column;

<img src="https://github.com/suadev/cdc-testing-with-pact-workshop/blob/main/_img/verified_contracts.png"></img>

## Next?

Try to break contracts between the services. Pact Broker shows the broken contracts with red background.

Following Pact list means; 'Product Service just made a change that breaks the contract with Price Service. Search service is not affected from this change.'

<img src="https://github.com/suadev/cdc-testing-with-pact-workshop/blob/main/_img/broken_contract.png"></img>

