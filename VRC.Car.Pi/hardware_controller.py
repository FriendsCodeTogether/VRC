import sys
import threading
from time import sleep
from ctypes import c_char
import board
import busio
import adafruit_ssd1306
import i2c_constants

class HardwareController:

  _i2c_lock = threading.RLock()
  _i2c = busio.I2C(board.SCL, board.SDA)
  _atmega1 = 0x20
  _atmega2 = 0x30
  _display = adafruit_ssd1306.SSD1306_I2C(128, 64, _i2c, addr=0x3c)
  _acceleration_sensor = 0x53
  _acceleration_sensor_value = 0
  _battery_percentage = 100

  def __init__(self):
    print("Initializing devices...")
    self.test_i2c_devices()

  def test_i2c_devices(self):
    print("Testing I2C devices...")
    bytesToSend = bytearray([32])
    try:
      self._i2c_lock.acquire()
      self._i2c.writeto(self._atmega1, bytesToSend)
      self._i2c_lock.release()
    except:
      sys.exit("Problem connecting to atmega1")

    try:
      self._i2c_lock.acquire()
      # self._i2c.writeto(self._atmega2, bytesToSend)
      self._i2c_lock.release()
    except:
      sys.exit("Problem connecting to atmega2")
    print("Testing I2C completed.")

  def send_car_command(self, carCommand):
    bytesToSend = bytearray([i2c_constants.MOTOR, ord(carCommand["direction"]), ord(carCommand["throttle"])])
    self._i2c_lock.acquire()
    self._i2c.writeto(self._atmega1, bytesToSend)
    self._i2c_lock.release()

  def read_acceleration_sensor(self):
    sys.exit("Not implemented yet")

  def read_color_sensor(self):
    bytesToSend = bytearray([i2c_constants.COLOR_SENSOR])
    readBuffer = bytearray([0] * 2)
    self._i2c_lock.acquire()
    self._i2c.writeto_then_readfrom(self._atmega1, bytesToSend, readBuffer)
    self._i2c_lock.release()
    return int.from_bytes(readBuffer, byteorder='little', signed=True)

  def read_ultrasonic_sensor(self):
    bytesToSend = bytearray([i2c_constants.ULTRASONIC_SENSOR])
    readBuffer = bytearray([0] * 2)
    self._i2c_lock.acquire()
    self._i2c.writeto_then_readfrom(self._atmega1, bytesToSend, readBuffer)
    self._i2c_lock.release()
    return int.from_bytes(readBuffer, byteorder='little', signed=True)

  def read_light_sensor(self):
    bytesToSend = bytearray([i2c_constants.ULTRASONIC_SENSOR])
    readBuffer = bytearray([0])
    self._i2c_lock.acquire()
    self._i2c.writeto_then_readfrom(self._atmega1, bytesToSend, readBuffer)
    self._i2c_lock.release()
    return bool.from_bytes(readBuffer, byteorder='little', signed=False)

  def set_buzzer(self, value):
    bytesToSend = bytearray([i2c_constants.BUZZER, value])
    self._i2c_lock.acquire()
    self._i2c.writeto(self._atmega1, bytesToSend)
    self._i2c_lock.release()

  def clear_screen(self):
    self._display.fill(0)
    self._display.show()

  def display_status(self):
    self.clear_screen()
    self._display.text('Status: {}'.format('connected'), 0, 0, 1)
    self._display.text('Battery percentage: {}%'.format(self._battery_percentage), 0, 0, 1)
    self._display.show()

