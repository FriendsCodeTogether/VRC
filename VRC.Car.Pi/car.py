from time import sleep
from hardware_controller import HardwareController
from messaging_handler import MessagingHandler

hardwareController = HardwareController()
messagingHandler = MessagingHandler()
messagingHandler.connect()
# hardwareController.send_car_command(5)

# Main loop
while True:
  # print(hardwareController.read_color_sensor())
  # print(hardwareController.read_ultrasonic_sensor())
  # print(hardwareController.read_light_sensor())
  sleep(1)
  hardwareController.set_buzzer(1)
  sleep(1)
  hardwareController.set_buzzer(0)
