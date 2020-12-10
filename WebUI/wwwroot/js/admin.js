var userId = getCookieValue('AnonymousUserId');
var prepareRaceButton = document.getElementById("prepare-race-btn");
var startRaceButton = document.getElementById("start-race-btn");
var stopRaceButton = document.getElementById("stop-race-btn");
const connection = new signalR.HubConnectionBuilder().withUrl('/racinghub').configureLogging(signalR.LogLevel.Information).build();


prepareRaceButton.addEventListener("click", () => prepareRaceButtonClicked());
startRaceButton.addEventListener("click", () => startRaceButtonClicked());
stopRaceButton.addEventListener("click", () => stopRaceButtonClicked());

async function prepareRaceButtonClicked() {
  prepareRaceButton.setAttribute("disabled", true);
  startRaceButton.removeAttribute("disabled");
  var lapAmount = 0;
  try {
    await connection.invoke('PrepareRaceAsync', lapAmount);
  } catch (e) {
    console.error(e);
  }

}

function startRaceButtonClicked() {
  startRaceButton.setAttribute("disabled", true);
  stopRaceButton.removeAttribute("disabled");

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
