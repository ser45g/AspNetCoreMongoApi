# App description

## In this project I just wanted to work a little bit with the Mongo Database. In the end, I actually used EF Core for that. Also, in this project I tried using different logging systems (ELK, Prometheus+Loki+Grafana, Aspire Dashboard, Seq, Jaeger dashboard), also, I used minimal apis with Carter at first, then I used the FastEndpoints library (I really should've put it on a different branch). Then I added Kubernetes deploy configs, then CI/CD with github. It also uses Problem Details and Global error handling.

**To deploy our app to Kubernetes:**

`cd ./kubernetes`
`make main`

**I used Gateway API for our cluster. Also I shared certificates with the services so that the can be accessed through https:**

![](/readme_img/weather-forecast.png)

![](/readme_img/mongo-express.png)

**I didn't manage to get seq working through Helm. It feels like not all configurations are set from the values file. Even though I used the `helm show values datalust/seq` command, it doesn't work. I get:

![](/readme_img/seq.png)


**Anyway, I used HeadLamp to have some gui to work with Kubernetes, these are the deployments and services that I have:**

![](/readme_img/headlamp-deployments.png)

![](/readme_img/headlamp-services.png)