import React, { useRef } from "react";
import { Card } from "react-bootstrap";
import { BackToLog } from "../ui-components/BackToLog";
import VideoImage from "../../assests/images/video-cover.png";
import VideoPlayerComponentStyle from "../../stylesheets/VideoPlayerComponentStyle.module.css";
import { VideoJS } from "../ui-components/VideoJs";
import { InvertedButton } from "../ui-components/InvertedButton";
import { EyebrowTitle } from "../ui-components/EyebrowTitle";

export const VideoPlayerComponent = () => {
  const overlayRef = useRef(null);
  const handleVideoStateChange = (isPlaying) => {
    if (overlayRef.current) {
      overlayRef.current.style.display = isPlaying ? "none" : "block";
    }
  };
  const videoJsOptions = {
    autoplay: false,
    controls: true,
    responsive: true,
    loop: true,
    muted: true,
    preload: "auto",
    fluid: true,
    poster: VideoImage,
    sources: [
      {
        src: "https://archive.org/download/BigBuckBunny_124/Content/big_buck_bunny_720p_surround.mp4",
        type: "video/mp4",
      },
    ],
  };
  return (
    <div>
      <Card style={{}} className={`p-3 ${VideoPlayerComponentStyle.main_card}`}>
        <Card.Body
          className={`p-0 ${VideoPlayerComponentStyle.main_card_body}`}
        >
          <VideoJS
            className={`${VideoPlayerComponentStyle.video}`}
            options={videoJsOptions}
            onStateChange={handleVideoStateChange}
          />
          <div ref={overlayRef}>
            <div
              id="top_overlay_text"
              className={`${VideoPlayerComponentStyle.bottom_overlay_text}`}
            >
              <div className="p-3">
                <div className="text-start d-flex justify-content-between align-items-baseline">
                  <EyebrowTitle
                    leftBorderColor="border_left_green"
                    title="THRIVR"
                    titleColor="color-FFFFFF"
                  />
                </div>
                <div className=" pt-3 fs-3 color-FFFFFF text-start">
                  What happens when consumers search for your business?
                </div>
                <div className="float-start pt-5">
                  <InvertedButton
                    name="NACS Magazine"
                    siconType="fa-regular"
                    siconName="fa-arrow-right-long"
                    siconColor="color-0053A5"
                  />
                </div>
              </div>
            </div>
          </div>
        </Card.Body>
        <div className="mt-5">
          <BackToLog />
        </div>
      </Card>
    </div>
  );
};
