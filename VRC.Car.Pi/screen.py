import board
import busio
import adafruit_ssd1306
from time import sleep

i2c = busio.I2C(board.SCL, board.SDA)
display = adafruit_ssd1306.SSD1306_I2C(128, 64, i2c, addr=0x3c)

display.fill(0)
display.text('Hello', 0, 0)
display.text('World', 0, 10)
display.show()

while True:
  sleep(1)
