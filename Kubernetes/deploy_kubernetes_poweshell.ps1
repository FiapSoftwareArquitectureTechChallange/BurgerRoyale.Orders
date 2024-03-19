Write-Host "Apply deployments e services"
kubectl apply -f api-deployment.yaml
kubectl apply -f api-svc.yaml

kubectl apply -f metricserver-deployment.yaml

Write-Host "Apply scale objects (HPA)"
kubectl apply -f api-scaleobject.yaml

Write-Host "Os recursos do Kubernetes foram aplicados."