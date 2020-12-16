import time
import board
import busio

# Create the I2C interface.
i2c = busio.I2C(board.SCL, board.SDA)


def set_buzzer(value):
  i2c. writeto(0x20, ['b', 1])
