/*
 * File: StepWizard.js
 * Author: Perera M.S.D/IT20020262
 */

import React from 'react'

const StepWizard = ({ currentActive, leftStepperTitle, rightStepperTitle }) => {
  return (
    <div className="step-wizard">
      <div className="step">
        <div
          className={`step-circle ${currentActive === 1 ? 'active' : ''}`}
          style={
            currentActive === 1
              ? { backgroundColor: '#7E5AE9' }
              : { backgroundColor: '#7E5AE9', opacity: 0.4 }
          }
        >
          1
        </div>
        <div
          className="step-title"
          style={currentActive === 1 ? {} : { opacity: 0.4 }}
        >
          {leftStepperTitle}
        </div>
      </div>
      <div
        className="step-line"
        // style={currentActive === 2 ? { left: 0 } : {}}
      />
      <div className="step">
        <div
          className={`step-circle ${currentActive === 2 ? 'active' : ''}`}
          style={
            currentActive === 2
              ? { backgroundColor: '#7E5AE9' }
              : { backgroundColor: '#7E5AE9', opacity: 0.4 }
          }
        >
          2
        </div>
        <div
          className="step-title"
          style={currentActive === 2 ? {} : { opacity: 0.4 }}
        >
          {rightStepperTitle}
        </div>
      </div>
      {/* <div className="step-content">
        {currentStep === 1 && <div>Step 1 Content</div>}
        {currentStep === 2 && <div>Step 2 Content</div>}
      </div> */}
    </div>
  )
}

export default StepWizard
