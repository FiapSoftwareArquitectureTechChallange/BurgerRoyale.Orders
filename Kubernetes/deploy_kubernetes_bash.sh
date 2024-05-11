#!/bin/bash

export PATH=$PATH:/c/Windows/system32

echo "Apply deployments e services"
"$kubectl" apply -f api-deployment.yaml
"$kubectl" apply -f api-svc.yaml

"$kubectl" apply -f metricserver-deployment.yaml

echo "Apply scale objects (HPA)"
"$kubectl" apply -f api-scaleobject.yaml

echo "Os recursos do Kubernetes foram aplicados."