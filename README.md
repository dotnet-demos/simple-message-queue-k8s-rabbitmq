# Simple message queue based processing hosted in Kubernetes.
A basic implementation of [Queue-Based Load Leveling](https://docs.microsoft.com/en-us/azure/architecture/patterns/queue-based-load-leveling) pattern using RabbitMQ that is hosted in Kubernetes

# Running using docker-compose

Run the below commands from the root of the repository
- docker-compose build
- docker-compose up --remove-orphans

# Running using Kubernetes

- Refer the [commands.ps1](./commands.ps1) file. It has commands to build and push images to repo as well as the commands to deploy to K8s

# Understanding how it works
- The webapi has actions to queue message to the RabbitMQ
  - It has no custom UI. We can either use Postman or navigate to the /swagger url to queue messages.
- The message-processor will de-queue messages and output to console. 
- Based on where it's running we can see the console output.
  - If it's running via docker-compose, the windows shows the logs automatically.
  - If it's running in K8s, refer the [commands.ps1](./commands.ps1) file to get commands to view logs. 
