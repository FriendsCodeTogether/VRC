from time import sleep
from hardware_controller import HardwareController
from messaging_handler import MessagingHandler

hardwareController = HardwareController()
messagingHandler = MessagingHandler(hardwareController)
messagingHandler.connect()

# Main loop
while True:
  print("Color Sensor: " + str(hardwareController.read_color_sensor()))
  print("Ultrasonic Sensor: " + str(hardwareController.read_ultrasonic_sensor()))
  print("Light Sensor: " + str(hardwareController.read_light_sensor()))
  sleep(5)
  # hardwareController.set_buzzer(1)
  # sleep(1)
  # hardwareController.set_buzzer(0)
