# Kubeless .NET Core Runtime

This is a .NET Core 2.0 runtime for the serverless functions framework [Kubeless](https://github.com/kubeless/kubeless).
The functions are provided as source code and are compiled when the docker container (a k8s pod) for the function is initialized.

## Http Functions

Functions triggered by the http-trigger can be implemented using the async/await keywords, but this is not mandatory.
By supporting async/await you are able to use APIs returning a Task/Task<T>.

## Supported Function Triggers

* :+1: HTTP-trigger
* PubSub-trigger (coming soon)
* Cron-trigger (coming soon)
