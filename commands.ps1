docker build -t joymon/message-queue-api:0.0.1 -f .\webapi\Dockerfile .
docker push joymon/message-queue-api:0.0.1

docker build -t joymon/message-queue-processor:0.0.1 -f .\message-processor\Dockerfile .
docker push joymon/message-queue-processor:0.0.1

kubectl apply -f .\webapi\k8s-docker-desktop-message-queue.yml

kubectl get all -n=message-queue-k8s

kubectl describe service/webapiservice -n=message-queue-k8s
kubectl describe pod/message-processor-5dc5d94c98-t5pbf -n=message-queue-k8s
kubectl logs pod/rabbitmq-57cbfb7848-t9wd8 -n=message-queue-k8s
kubectl logs pod/webapi-66cb46499-fnj4h -n=message-queue-k8s
kubectl logs pod/message-processor-5dc5d94c98-t5pbf -n=message-queue-k8s

# Clean up
kubectl delete -f .\webapi\k8s-docker-desktop-message-queue.yml