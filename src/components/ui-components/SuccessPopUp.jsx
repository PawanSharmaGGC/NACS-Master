import React, { useEffect, useState } from "react";
import { Toast, ToastContainer } from "react-bootstrap";

export const SuccessPopUp = ({ showFlag, setShowAlertFlag }) => {
  const [show, setShow] = useState(showFlag);

  useEffect(() => {
    setShow(showFlag);
  }, [showFlag]);

  useEffect(() => {
    setTimeout(() => {
      setShow(false);
      setShowAlertFlag(false);
    }, 5000);
  }, [show]);

  return (
    <div>
      <div aria-live="polite" aria-atomic="true" className="position-relative">
        <ToastContainer
          className="p-3 position-absolute top-0 end-0 z-1"
          position="top-end"
        >
          <Toast className="bg-DBEAB9 text-light" show={show}>
            <Toast.Body className="p-3">
              <p className="color-0053A5 fs-5 text-start m-0 mb-2">
                Form Submitted!
              </p>
              <p className="color-000000 text-start m-0">
                You have signed up for the webinar. See you there!
              </p>
            </Toast.Body>
          </Toast>
        </ToastContainer>
      </div>
    </div>
  );
};
