var userId = getCookieValue('AnonymousUserId');
var prepareRaceButton = document.getElementById("prepare-race-btn");
var startRaceButton = document.getElementById("start-race-btn");
var stopRaceButton = document.getElementById("stop-race-btn");
var lapselect = document.getElementById("admin-lap-selector-select");
const connection = new signalR.HubConnectionBuilder().withUrl('/racinghub').configureLogging(signalR.LogLevel.Information).build();


prepareRaceButton.addEventListener("click", () => prepareRaceButtonClicked());
startRaceButton.addEventListener("click", () => startRaceButtonClicked());
stopRaceButton.addEventListener("click", () => stopRaceButtonClicked());

async function prepareRaceButtonClicked() {
  prepareRaceButton.setAttribute("disabled", true);
  startRaceButton.removeAttribute("disabled");
  //var lapAmount = 1;
  var lapAmount = lapselect.value;
  console.log(lapAmount);
  try {
    await connection.invoke('PrepareRaceAsync', parseInt(lapAmount));
  } catch (e) {
    console.error(e);
  }

}

async function startRaceButtonClicked() {
  startRaceButton.setAttribute("disabled", true);
  stopRaceButton.removeAttribute("disabled");
  try {
    await connection.invoke('StartRaceAsync');
  } catch (e) {
    console.error(e);
  }
}

function stopRaceButtonClicked() {
  stopRaceButton.setAttribute("disabled", true);
  prepareRaceButton.removeAttribute("disabled");

}

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
