import board
import busio
import adafruit_ssd1306
from time import sleep

i2c = busio.I2C(board.SCL, board.SDA)
display = adafruit_ssd1306.SSD1306_I2C(128, 64, i2c, addr=0x3c)

display.fill(0)

display.show()

# Set a pixel in the origin 0,0 position.
display.pixel(0, 0, 1)
# Set a pixel in the middle 64, 16 position.
display.pixel(64, 32, 1)
# Set a pixel in the opposite 127, 31 position.
display.pixel(127, 63, 1)
display.show()

while True:
  for x in range(127):
    for y in range(63):
      display.pixel(x - 1, y - 1, 0)
      display.pixel(x, y, 1)
      display.show()
      sleep(0.2)
  sleep(1)
