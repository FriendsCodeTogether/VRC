from time import sleep
from signalrcore.hub_connection_builder import HubConnectionBuilder
from hardware_controller import HardwareController
import netifaces

class MessagingHandler:
  _hubUrl = 'https://192.168.0.201:5001/racinghub'
  _hubConnection = HubConnectionBuilder()\
    .with_url(_hubUrl, options={"verify_ssl": False}) \
    .build()
  _reconnect = False

  carNumber = 1

  def __init__(self, hardwareController):
    self._hardwareController = hardwareController
    self._hubConnection.on_open(self.on_connect)
    self._hubConnection.on_close(self.on_close)
    self._hubConnection.on_error(self.on_disconnect)
    self._hubConnection.on("ReceiveCarCommand", self.on_receive_car_command)
    self._hubConnection.on("AssignCarNumber", self.on_receive_car_number)

  def connect(self):
    print('Connecting to API at \"{}\"...'.format(self._hubUrl))
    self._hardwareController.display_text('Connecting to API')
    sleep(0.3)
    try:
      self._hubConnection.start()
    except:
      print('Failed to connect to API')
      self._hardwareController.display_text('Failed to connect to API')
      self._hardwareController.connection_status = 'error'
      sleep(0.3)

  def on_connect(self):
    print('Connected to API with ip \"{}\"'.format(self.get_ip_address()))
    self._hardwareController.connection_status = 'connected'
    if not self._reconnect:
      self.request_car_number()
    else:
      self.reclaim_car_number()

  def on_close(self):
    print("connection closed")
    self.stop_car()
    self._hardwareController.connection_status = 'disconnected'

  def on_disconnect(self):
    print("Connection lost")
    self.stop_car()
    self._hardwareController.connection_status = 'disconnected'
    self._reconnect = True
    self.connect()

  def on_receive_car_command(self, carCommand):
    self._hardwareController.send_car_command(carCommand[0])

  def stop_car(self):
    print('Stopping car')
    stopCommand = {
      'carNumber': self.carNumber,
      'direction': 'N',
      'throttle': 'N'
    }
    self._hardwareController.send_car_command(stopCommand)

  def on_receive_car_number(self, carNumber):
    print('Received car number')
    print('Our new car number: {}'.format(carNumber[0]))
    self.carNumber = carNumber[0]
    self._hardwareController.car_number = carNumber[0]

  def request_car_number(self):
    print('Requesting car number...')
    self._hubConnection.send("RequestCarNumber", [self.get_ip_address()])

  def reclaim_car_number(self):
    self._hubConnection.send("ReclaimCarNumber", [self.carNumber, self.get_ip_address()])

  def send_car_command(self):
    print('Sending car command to myself for testing...')
    command = {
      'carNumber': 1,
      'Direction': 'L',
      'Throttle': 'F'
    }
    self._hubConnection.send("SendCarCommand", [self.carNumber, command])

  def get_ip_address(self):
    ipAddress = netifaces.ifaddresses('wlan0')[2][0]['addr']
    print(ipAddress)
    return ipAddress
