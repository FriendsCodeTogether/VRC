from flask import Flask
import os
import sys

app = Flask(__name__)


@app.route('/')
def index():
    return 'this is flask'


@app.route('/music')
def music():
  fifo = open('video_fifo', "rb")
  fifo.close()
  return True



if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0')
