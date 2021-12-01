# Practice 4
## Setup and run the project:
1. Install docker-compose
2. In the root directory run ```docker-compose up --build``` and wait for services to launch.
3. Open http://localhost:7170/swagger 
    1. Now you can test the API by making POST and GET requests
    2. You must specify the Id of entity yourself, so that you can retreive it later using GET request.
    3. User entities use GUID as the id type which you can generate with ```/guid``` endpoint.
    4. When you GET entity, Api service checks cache for entity and if it is not present loads it from Data service while populating the cache.
4. After testing close with ```Ctrl+C```
5. To cleanup all containers run ```docker-compose down -v```
