create-secrets-for-tls:
	kubectl create secret generic tls-seq-certificates-secret --from-file=443.pfx=rootCA.pfx --from-file=45341.pfx=rootCa.pfx

	kubectl create configmap tls-key-and-certificate-configmap --from-file=ca.crt=rootCa.crt --from-file=ca.key=rootCa.key

	kubectl create secret generic tls-pfx-password-secret --from-literal=password=550591

	kubectl create secret generic tls-pfx-certificate-secret --from-file=pfx-cert=rootCA.pfx  

	kubectl create secret tls tls-gateway-key-and-certificate-secret --cert=rootCa.crt --key=rootCa.key
