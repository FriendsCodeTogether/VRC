from flask import Flask

app = Flask(__name__)


@app.route('/')
def index():
    return 'this is flask'


@app.route('/music')
def music():
    import test
    return test.cameraView()


if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0')
