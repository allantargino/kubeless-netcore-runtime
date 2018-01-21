build:
	docker build . -t=kubeless-dotnetcore-runtime

run:
	docker run -it \
	-e MOD_NAME='hasher' \
	-e FUNC_HANDLER='execute' \
	-e DOTNETCORESHAREDREF_VERSION='2.0.5' \
	-e ASPNETCORE_ENVIRONMENT='Development' \
	-p 8080:8080 \
	-v `pwd`/examples:/kubeless \
	kubeless-dotnetcore-runtime /bin/bash