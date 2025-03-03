import React, { useRef, useState } from "react";
import { Card } from "react-bootstrap";
import { BackToLog } from "../ui-components/BackToLog";
import EventVideoCardStyle from "../../stylesheets/EventVideoCardStyle.module.css";
import { VideoJS } from "../ui-components/VideoJs";
import EventVideoCardImage from "../../assests/images/tier-1-content-img.png";
import { EyebrowTitle } from "../ui-components/EyebrowTitle";
import { Tag } from "../ui-components/Tag";

export const EventVideoCard = () => {
  const [player, setPlayer] = useState(null);
  const overlayRef = useRef(null);

  const videoJsOptions = {
    autoplay: false,
    controls: true,
    responsive: true,
    loop: false,
    muted: true,
    preload: "auto",
    fluid: true,
    poster: EventVideoCardImage,
    sources: [
      {
        src: "https://archive.org/download/BigBuckBunny_124/Content/big_buck_bunny_720p_surround.mp4",
        type: "video/mp4",
      },
    ],
  };

  // Callback to update overlay visibility
  const handleVideoStateChange = (isPlaying) => {
    if (overlayRef.current) {
      overlayRef.current.style.display = isPlaying ? "none" : "block";
    }
  };

  const handlePlayerReady = (playerInstance) => {
    setPlayer(playerInstance);
  };

  return (
    <div>
      <Card className={`p-2 ${EventVideoCardStyle.main_card}`}>
        <Card className={`border ${EventVideoCardStyle.section_card}`}>
          <Card.Body className="p-0">
            <div>
              <VideoJS
                className={`${EventVideoCardStyle.section_card}`}
                options={videoJsOptions}
                onClick={handlePlayerReady}
                onStateChange={handleVideoStateChange}
              />
            </div>

            <div ref={overlayRef}>
              <div
                id="top_overlay_text"
                className={`${EventVideoCardStyle.top_overlay_text}`}
              >
                <div className="p-2">
                  <div className="text-start align-items-baseline d-flex justify-content-between">
                    <EyebrowTitle
                      leftBorderColor="border_left_white"
                      title="featured"
                      titleColor="color-FFFFFF"
                    />
                    <Tag
                      tagName="NEW"
                      bgColor="bg-DBEAB9"
                      textColor="color-0053A5"
                    />
                  </div>
                </div>
              </div>
              <div
                id="bottom_overlay_text"
                className={`${EventVideoCardStyle.bottom_overlay_text}`}
              >
                <div className="p-3">
                  <p className="fs-5 fw-lighter mb-0 color-FFFFFF bottom-0 end-0">
                    Watch the NACS Show ‘23 Highlight Reel.
                  </p>
                </div>
              </div>
            </div>
          </Card.Body>
        </Card>
        <div className="mt-5">
          <BackToLog />
        </div>
      </Card>
    </div>
  );
};
