import React, { useEffect, useState } from "react";
import CountdownTimerStyle from "../../stylesheets/CountdownTimerStyle.module.css";

export const CountdownTimer = ({ eventDate }) => {
  const [timeRemaining, setTimeRemaining] = useState(0);

  useEffect(() => {
    if (eventDate) {
      const countdownInterval = setInterval(() => {
        const currentTime = new Date().getTime();
        const eventTime = new Date(eventDate).getTime();
        let remainingTime = eventTime - currentTime;

        if (remainingTime <= 0) {
          remainingTime = 0;
          clearInterval(countdownInterval);
          alert("Countdown complete!");
        }
        setTimeRemaining(remainingTime);
      }, 1000);

      return () => clearInterval(countdownInterval);
    }
  }, [eventDate, timeRemaining]);

  const seconds = Math.floor((timeRemaining / 1000) % 60);
  const minutes = Math.floor((timeRemaining / (1000 * 60)) % 60);
  const hours = Math.floor((timeRemaining / (1000 * 60 * 60)) % 24);
  const days = Math.floor(timeRemaining / (1000 * 60 * 60 * 24));

  return (
    <div className="ps-lg-5 pt-lg-0 pt-1 text-start mt-5">
      <p className="fs-4 color-3C5567 mb-4 ">Countdown:</p>
      <p className="d-flex mb-4 fs-5">
        <div
          className={`me-4 me-lg-5 fw-bold ${CountdownTimerStyle.countdown_brdr}`}
        >
          <span className="color-002569 fs-3">
            {days.toString().padStart(2, "0")}
          </span>
          <span className="color-B9D300 fs-3">d</span>
        </div>
        <div
          className={`me-4 me-lg-5 fw-bold ${CountdownTimerStyle.countdown_brdr}`}
        >
          <span className="color-002569 fs-3">
            {hours.toString().padStart(2, "0")}
          </span>
          <span className="color-B9D300 fs-3">h</span>
        </div>
        <div
          className={`me-4 me-lg-5 fw-bold ${CountdownTimerStyle.countdown_brdr}`}
        >
          <span className="color-002569 fs-3">
            {minutes.toString().padStart(2, "0")}
          </span>
          <span className="color-B9D300 fs-3">m</span>
        </div>
        <div
          className={`me-4 me-lg-5 fw-bold ${CountdownTimerStyle.countdown_brdr}`}
        >
          <span className="color-002569 fs-3">
            {seconds.toString().padStart(2, "0")}
          </span>
          <span className="color-B9D300 fs-3">s</span>
        </div>
      </p>
    </div>
  );
};
