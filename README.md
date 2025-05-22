# hangfire-test

To test the whole app you have to start Web, Server.Console and Worker.Console

## Web
This is the web gui that has some buttons for triggering calculations. 
It's also hosting the hangfire dashboard at '/hangfire'

##Server.Console
This is a Hangfire "Agent" that picks up jobs from the queue and executes them. By default it is using an implementation that calls a http endpoint (Worker.Console)

##Worker.Console
Self hosted http endpoint that listens for calls on '/add/{double}/{double}' and returns the results. Can be configured to sleep before returning the results

This works# Test commit for CI workflow
