// Driving constants
const neutral = 0;
const forward = 1;
const backward = -1;
const left = -1;
const right = 1;

// Car command properties
var carNumber = 1;
var direction = 0;
var throttle = 0;

// User info
var userId = getCookieValue('AnonymousUserId');

function handleUpArrowDown() {
  document.getElementById('control-panel-up').src = '/Images/ButtonUpUsed.png';
  throttle = forward;
}

function handleUpArrowUp() {
  document.getElementById('control-panel-up').src = '/Images/ButtonUpDefault.png';
  throttle = neutral;
}

function handleDownArrowDown() {
  document.getElementById('control-panel-down').src = '/Images/ButtonDownUsed.png';
  throttle = backward;
}

function handleDownArrowUp() {
  document.getElementById('control-panel-down').src = '/Images/ButtonDownDefault.png';
  throttle = neutral;
}

function handleLeftArrowDown() {
  document.getElementById('control-panel-left').src = '/Images/ButtonLeftUsed.png';
  direction = left;
}

function handleLeftArrowUp() {
  document.getElementById('control-panel-left').src = '/Images/ButtonLeftDefault.png';
  direction = neutral;
}

function handleRightArrowDown() {
  document.getElementById('control-panel-right').src = '/Images/ButtonRightUsed.png';
  direction = right;
}

function handleRightArrowUp() {
  document.getElementById('control-panel-right').src = '/Images/ButtonRightDefault.png';
  direction = neutral;
}

async function sendCarCommand() {
  let carCommand = {
    carNumber: carNumber,
    throttle: throttle,
    direction: direction,
  };

  try {
    await connection.invoke('SendCarCommand', 1, carCommand);
  } catch (err) {
    console.error(err);
  }
}

document.addEventListener('keydown', function (e) {
  if (e.repeat) {
    return;
  }

  switch (e.keyCode) {
    case 37:
      //left arrow
      handleLeftArrowDown();
      break;
    case 38:
      //up arrow
      handleUpArrowDown();
      break;
    case 39:
      //right arrow
      handleRightArrowDown();
      break;
    case 40:
      //down arrow
      handleDownArrowDown();
      break;
  }
  sendCarCommand();
});

document.addEventListener('keyup', function (e) {
  switch (e.keyCode) {
    case 37:
      //left arrow
      handleLeftArrowUp();
      break;
    case 38:
      //up arrow
      handleUpArrowUp();
      break;
    case 39:
      //right arrow
      handleRightArrowUp();
      break;
    case 40:
      //down arrow
      handleDownArrowUp();
      break;
  }
  sendCarCommand();
});

const connection = new signalR.HubConnectionBuilder()
  .withUrl('/messaginghub')
  .configureLogging(signalR.LogLevel.Information)
  .build();

async function start() {
  try {
    await connection.start();
    console.log('SignalR Connected.');
  } catch (err) {
    console.log(err);
    setTimeout(start, 5000);
  }
}

connection.onclose(start);

// Start the connection.
start();
