create_secret_for_tls_certificates:
	kubectl create secret generic mypfx --from-file=pfx-cert=rootCA.pfx --from-file=crt=rootCa.crt --from-file=key=rootCa.key

create_configmap_for_tls_verification:
	kubectl create configmap backend-cert --from-file=ca.crt=rootCA.crt