import board
import busio
import adafruit_ssd1306

i2c = busio.I2C(board.SCL, board.SDA)
display = adafruit_ssd1306.SSD1306_I2C(128, 32, i2c, addr=0x3c)

display.fill(0)

display.show()

