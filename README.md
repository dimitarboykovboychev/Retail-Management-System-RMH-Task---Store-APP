# Retail Management System (RMH Task) - Store APP

This project uses MassTransit with RabbitMQ as the message broker. To run the apps locally, you’ll need Docker installed.

Start RabbitMQ in Docker
Run this command in your terminal:

docker run -d --hostname my-rabbit --name rabbitmq \ -p 5672:5672 -p 15672:15672 rabbitmq:3-management

powershell: docker run -d --hostname my-rabbit --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management

5672 → broker port (MassTransit connects here)

15672 → management UI (open http://localhost:15672)

Default login: guest / guest

Run the apps
Start the CentralApp and StoreApp.

They will connect to RabbitMQ automatically (configured in Program.cs).

You can publish and consume messages across apps.

Verify messages
Open the RabbitMQ Management UI at http://localhost:15672.

Navigate to Queues → you’ll see messages flowing in/out.

Consumers should log activity when messages are processed.
