from signalrcore.hub_connection_builder import HubConnectionBuilder
from hardware_controller import HardwareController

class MessagingHandler:
  _hubUrl = 'https://192.168.0.100:5001/racinghub'
  _hubConnection = HubConnectionBuilder()\
    .with_url(_hubUrl, options={"verify_ssl": False}) \
    .with_automatic_reconnect({
            "type": "interval",
            "keep_alive_interval": 10,
            "intervals": [1, 3, 5, 6, 7, 87, 3]
        }).build()

  carNumber = 1

  def __init__(self, hardwareController):
    self._hardwareController = hardwareController
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

  def on_receive_car_command(self, carCommand):
    self._hardwareController.send_car_command(carCommand[0])

  def on_receive_car_number(self, carNumber):
    print('Received car number')
    print('Our new car number: {}'.format(carNumber[0]))
    self.carNumber = carNumber[0]

  def request_car_number(self):
    print('Requesting car number...')
    self._hubConnection.send("RequestCarNumber", [0])

  def send_car_command(self):
    print('Sending car command to myself for testing...')
    command = {
      'carNumber': 1,
      'Direction': 'L',
      'Throttle': 'F'
    }
    self._hubConnection.send("SendCarCommand", [self.carNumber, command])

