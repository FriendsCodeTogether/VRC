from signalrcore.hub_connection_builder import HubConnectionBuilder
from event_hook import EventHook

class MessagingHandler:
  _hubUrl = 'https://localhost:5001/racinghub'
  _hubConnection = HubConnectionBuilder()\
    .with_url(_hubUrl, options={"verify_ssl": False}) \
    .with_automatic_reconnect({
            "type": "interval",
            "keep_alive_interval": 10,
            "intervals": [1, 3, 5, 6, 7, 87, 3]
        }).build()

  carNumber = 0
  carCommandReceivedEvent = EventHook(carCommand = dict)

  def __init__(self):
    self._hubConnection.on_open(self.on_connect)
    self._hubConnection.on_close(lambda: print("connection closed"))
    self._hubConnection.on("ReceiveCarCommand", self.on_receive_car_command)
    self._hubConnection.on("AssignCarNumber", self.on_receive_car_number)

  def connect(self):
    print('Connecting to API at \"{}\"...'.format(self._hubUrl))
    try:
      self._hubConnection.start()
    except:
      print('Failed to connect to API');

  def on_connect(self):
    print('Connected to API')
    self.request_car_number()
    self.send_car_command()

  def on_receive_car_command(self, carCommand):
    print(carCommand[0])
    self.carCommandReceivedEvent(carCommand[0])

  def on_receive_car_number(self, carNumber):
    print('Received car number')
    print('Our new car number: {}'.format(carNumber[0]))
    self.carNumber = carNumber[0]

  def request_car_number(self):
    print('Requesting car number...')
    self._hubConnection.send("RequestCarNumber", [0], lambda m: print(m))

  def send_car_command(self):
    print('Sending car command...')
    command = {
      'carNumber': 5,
      'Direction': 'L',
      'Throttle': 'F'
    }
    self._hubConnection.send("SendCarCommand", [1, command], lambda m: print(m))

