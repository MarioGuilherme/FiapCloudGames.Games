kubectl apply -f ./games-sqlserver.yaml
kubectl apply -f ./games-configmap.yaml
kubectl apply -f ./games-secret.yaml
kubectl apply -f ./games-api.yaml
kubectl apply -f ./games-service.yaml
kubectl apply -f ./games-hpa.yaml
minikube service games-api
