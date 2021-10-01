# message-queue-k8s
Message based queue processing system hosted in Kubernetes

# Running using docker-compose

Run the below commands from the root of the repository
- docker-compose build
- docker-compose up --remove-orphans

# Running using Kubernetes

- Refer the [commands.ps1](./commands.ps1) file. It has commands to build and push images to repo as well as the commands to deploy to K8s

# Understanding how it works
- The webapi has actions to queue message to the RabbitMQ
  - It has no custom UI. We can eitehr use Postman or navigate to the /swagger to queue messages.
- The message-processor will de-queue messages and output to console. 
- Based on where it's running we can see the console output.
