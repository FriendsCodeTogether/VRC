from signalrcore.hub_connection_builder import HubConnectionBuilder

class MessagingHandler:
  _hubUrl = 'https://localhost:5001/racinghub'
  _hubConnection = HubConnectionBuilder()\
    .with_url(_hubUrl, options={"verify_ssl": False}) \
    .with_automatic_reconnect({
            "type": "interval",
            "keep_alive_interval": 10,
            "intervals": [1, 3, 5, 6, 7, 87, 3]
        }).build()

  def __init__(self):
    self._hubConnection.on_open(lambda: print("connection opened and handshake received ready to send messages"))
    self._hubConnection.on_close(lambda: print("connection closed"))
    self._hubConnection.on("ReceiveMessage", print)

  def connect(self):
    self._hubConnection.start()

