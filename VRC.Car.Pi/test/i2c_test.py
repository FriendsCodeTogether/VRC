from time import sleep
import board
import busio

# Create the I2C interface.
i2c = busio.I2C(board.SCL, board.SDA)


def set_buzzer(value):
  bytesToSend = bytearray([98, value])
  i2c.writeto(0x20, bytesToSend)


# Main loop
while True:
  sleep(1)
  set_buzzer(1)
  sleep(1)
  set_buzzer(0)
