// Driving constants
const neutral = 'N';
const forward = 'F';
const backward = 'B';
const left = 'L';
const right = 'R';

// SignalR constants
const racinghubUrl = '/racinghub';

// Car properties
var carNumber = 0;
var carIpAddress = '';
var direction = 'N';
var throttle = 'N';

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
    await connection.invoke('SendTestCarCommand', carNumber, carCommand);
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
  .withUrl(racinghubUrl)
  .configureLogging(signalR.LogLevel.Information)
  .build();

connection.on('showRaceCountdown', () => showRaceCountdown());
connection.on('UpdateRaceCountdownTime', (seconds) => UpdateRaceCountdownTime(seconds));
connection.on('RemoveRaceCountdown', () => RemoveRaceCountdown());
connection.on('RemoveRacers', () => removeRacers());

var countdown = document.getElementById('race-start-countdown');
var countdowntext = document.getElementById('race-start-countdown-text');
var playerNumber = document.getElementById('player-number');
var lapAmount = document.getElementById('lap-amount');
var cameraFeedDisplay = document.getElementById('racer-camera-feed');

function showRaceCountdown() {
  console.log('start Race coutdown');
  countdown.style.display = 'block';
}

function UpdateRaceCountdownTime(seconds) {
  console.log(seconds);
  countdowntext.innerHTML = seconds;
}

function RemoveRaceCountdown() {
  console.log('race started');
  countdown.style.display = 'none';
}

async function start() {
  try {
    await connection.start();
    console.log('SignalR Connected.');
    await connectRacerToCar();
    // await connection.invoke('PutRacerInGroupAsync');
    // getLapAmount();
  } catch (err) {
    console.log(err);
    setTimeout(start, 5000);
  }
}

async function getLapAmount() {
  var getLapAmount = await connection.invoke('GetLapAmount');
  console.log(getLapAmount);
  lapAmount.textContent = getLapAmount;
}

function removeRacers() {
  location.replace('/');
}

async function getCarIpAddress() {
  carIpAddress = await connection.invoke('GetCarIpAddress', carNumber);
  cameraFeedDisplay.src = `http://${carIpAddress}:8000/stream.mpg`;
}

async function connectRacerToCar() {
  var receivedCarNumber = 1;
  console.log(receivedCarNumber);
  if (receivedCarNumber != -1) {
    console.log('User connected to car');
    playerNumber.textContent = receivedCarNumber;
    carNumber = receivedCarNumber;
    await getCarIpAddress();
  } else {
    console.log('no cars available');
    location.replace('/');
  }
}

connection.onclose(start);

// Start the connection.
start();
