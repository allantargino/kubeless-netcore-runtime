build:
	docker build . -t=kubeless-dotnetcore-runtime

bash:
	docker run -it \
	--entrypoint /bin/bash \
	-e MOD_NAME='Hasher' \
	-e FUNC_HANDLER='Execute' \
	-p 8080:8080 \
	-v `pwd`/examples:/kubeless \
	kubeless-dotnetcore-runtime

run:
	docker run -it \
	-e MOD_NAME='Hasher' \
	-e FUNC_HANDLER='Execute' \
	-p 8080:8080 \
	-v `pwd`/examples:/kubeless \
	kubeless-dotnetcore-runtime