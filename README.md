# RedisCaching
### What is distributed caching and its benefit
 
Distributed caching is when you want to handle caching outside of your application. This also can be shared by one or more applications/servers. Distributed cache is application-specific; i.e., multiple cache providers support distributed caches. To implement distributed cache, we can use Redis and NCache. We will see about Redis cache in detail.
 
A distributed cache can improve the performance and scalability of an ASP.NET Core app, especially when the app is hosted by a cloud service or a server farm.
 
### Benefits
1. Data is consistent throughout multiple servers.
2. This is more suitable for microservice architecture
3. In case of loading balancing, this is recommended
4. Multiple Applications / Servers can use one instance of Redis Server to cache data. This reduces the cost of maintenance in the long run
IDistributedCache interface
 
## IDistributedCache Interface provides you with the following methods to perform actions on the actual cache
 - GetAsync - Gets the Value from the Cache Server based on the passed key.
 - SetAsync - Accepts a key and Value and sets it to the Cache server
 - RefreshAsync - Resets the Sliding Expiration Timer (more about this later in the article) if any.
 - RemoveAsync - Deletes the cache data based on the key.

### Framework provided to implement IDistributedCache 
Register an implementation of IDistributedCache in Startup.ConfigureServices. Framework-provided implementations described in this topic include
 - Distributed Memory Cache
 - Distributed SQL Server cache
 - Distributed Redis cache
 - Distributed NCache cache
 - Distributed Redis cache
 
Redis is an open-source in-memory data store, which is often used as a distributed cache. You can configure an Azure Redis Cache for an Azure-hosted ASP.NET Core app, and use an Azure Redis Cache for local development.
 
Redis supports quite a lot of data structures like string, hashed, lists, queries, and much more and it’s a fast key-value-based database that is written in C. It’s a NoSQL database as well. For this purpose, it is being used in Stackoverflow, Github, and so on.

#### DistributedCacheEntryOptions,
 - ```SetAbsoluteExpiration```
Here you can set the expiration time of the cached object.

 - ```SetSlidingExpiration```
This is similar to Absolute Expiration. It expires as a cached object if it not being requested for a defined amount of time period. Note that Sliding Expiration should always be set lower than the absolute expiration
