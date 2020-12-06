var userId = getCookieValue('AnonymousUserId');
var prepareRaceButton = document.getElementById("prepare-race-btn");
var startRaceButton = document.getElementById("start-race-btn");
var stopRaceButton = document.getElementById("stop-race-btn");


prepareRaceButton.addEventListener("click", () => prepareRaceButtonClicked());
startRaceButton.addEventListener("click", () => startRaceButtonClicked());
stopRaceButton.addEventListener("click", () => stopRaceButtonClicked());

function prepareRaceButtonClicked() {
  prepareRaceButton.setAttribute("disabled", true);
  startRaceButton.removeAttribute("disabled");

}

function startRaceButtonClicked() {
  startRaceButton.setAttribute("disabled", true);
  stopRaceButton.removeAttribute("disabled");

}

function stopRaceButtonClicked() {
  stopRaceButton.setAttribute("disabled", true);
  prepareRaceButton.removeAttribute("disabled");

}
