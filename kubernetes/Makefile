#!make
include ../.env

main: create-secrets-for-tls install-charts create-secrets-for-mongodb create-secrets-for-seq
	kubectl apply -f . -R

create-secrets-for-mongodb:
	kubectl create secret generic mongodb-secret --from-literal=mongo-root-username=${mongo-root-username} --from-literal=mongo-root-password=${mongo-root-password}

create-secrets-for-seq:
	kubectl create secret generic seq-secret --from-literal=password=${SEQ_FIRSTADMINLOGINPASSWORD}

create-secrets-for-tls:
	kubectl create secret generic tls-seq-certificates-secret --from-file=443.pfx=rootCA.pfx --from-file=45341.pfx=rootCa.pfx

	kubectl create configmap tls-key-and-certificate-configmap --from-file=ca.crt=rootCa.crt --from-file=ca.key=rootCa.key

	kubectl create secret generic tls-pfx-password-secret --from-literal=password=${tls-password}

	kubectl create secret generic tls-pfx-certificate-secret --from-file=pfx-cert=rootCA.pfx  

	kubectl create secret tls tls-gateway-key-and-certificate-secret --cert=rootCa.crt --key=rootCa.key

install-charts: update-repo install-seq install-nginx-gateway-fabric
	echo "Installed charts"
update-repo:
	helm repo update

install-seq:
	helm install seq datalust/seq --values helm/envs/production/values/seq.yaml

install-nginx-gateway-fabric:
	kubectl kustomize "https://github.com/nginx/nginx-gateway-fabric/config/crd/gateway-api/standard?ref=v2.4.2" | kubectl apply -f -
	helm install ngf oci://ghcr.io/nginx/charts/nginx-gateway-fabric --create-namespace -n nginx-gateway

aspnetcoremongoapi-build-dev:
	docker build -f AspNetCoreMongoApi/Dockerfile --target dev -t seralex915/weather-forecast-api .