# Deploy

# build and push images . Is the the tags are going to be changed, make sure the yaml is also changed.
docker build -t joymon/message-queue-api:0.0.1 -f .\webapi\Dockerfile .
docker push joymon/message-queue-api:0.0.1

docker build -t joymon/message-queue-processor:0.0.1 -f .\message-processor\Dockerfile .
docker push joymon/message-queue-processor:0.0.1

# Apply K8s file to the cluster. This require kubectl to be connected to local cluster or remote cluster. 

kubectl apply -f .\webapi\k8s-docker-desktop-message-queue.yml

# Debug scripts
kubectl get all -n=message-queue-k8s

kubectl describe service/webapiservice -n=message-queue-k8s
kubectl describe pod/message-processor-5dc5d94c98-t5pbf -n=message-queue-k8s
kubectl logs pod/rabbitmq-57cbfb7848-t9wd8 -n=message-queue-k8s
kubectl logs pod/webapi-66cb46499-fnj4h -n=message-queue-k8s
kubectl logs pod/message-processor-5dc5d94c98-lmkpc -n=message-queue-k8s

# Clean up
kubectl delete -f .\webapi\k8s-docker-desktop-message-queue.yml