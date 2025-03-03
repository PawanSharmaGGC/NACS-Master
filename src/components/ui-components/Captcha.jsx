import React from "react";
import Recaptcha from "../../assests/images/recaptcha-logo.png";
import { Form } from "react-bootstrap";
import CaptchaStyle from "../../stylesheets/CaptchaStyle.module.css";

export const Captcha = () => {
  return (
    <div
      className={`bg-FFFFFF align-middle mb-3 ps-2 pe- ms-3 me-3 ${CaptchaStyle.main_div}`}
    >
      <Form.Group
        className="d-flex align-items-center justify-content-between"
        id="formGridCheckbox"
      >
        <div className={`form-check ${CaptchaStyle.form_check}`}>
          <input
            className={`form-check-input ${CaptchaStyle.checkbox}`}
            type="checkbox"
            id="gridCheck"
          />
          <label className="form-check-label p-2" for="gridCheck">
            I'm not a robot
          </label>
        </div>

        <span className="pt-1 pe-3">
          <img
            src={Recaptcha}
            alt=""
            fluid
            className={`${CaptchaStyle.captcha_img}`}
          />
          <p className="fs-9">Privacy Terms</p>
        </span>
      </Form.Group>
    </div>
  );
};
