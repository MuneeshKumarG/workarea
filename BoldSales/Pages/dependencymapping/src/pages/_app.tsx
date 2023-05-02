import '@/styles/globals.css'
import type { AppProps } from 'next/app'
import { registerLicense } from "@syncfusion/ej2-base";
import "../../node_modules/@syncfusion/ej2-buttons/styles/tailwind.css";
import "../../node_modules/@syncfusion/ej2-base/styles/tailwind.css";
import "../../node_modules/@syncfusion/ej2-inputs/styles/tailwind.css";
import "../../node_modules/@syncfusion/ej2-react-dropdowns/styles/tailwind.css";

registerLicense(

    "Mgo+DSMBaFt+QHFqUUdrWU5Gc0BAXWFKblZ8RmZTel9gBShNYlxTR3ZbQFhjT35Tc0NnXH5c;Mgo+DSMBPh8sVXJ1S0d+WFBPc0BAWHxLflF1VWZTfFt6cVRWESFaRnZdQV1nS3tTcEVjXHlad3JX;ORg4AjUWIQA/Gnt2VFhhQlVFfVpdX2ZWfFN0RnNcdV11flBHcC0sT3RfQF5jTXxWdkNjWXpYdXRdRA==;MTgzNDIxMEAzMjMxMmUzMTJlMzQzMUxOa0RIS25KK1FkSGE5MUhUdzgydDZjZ2U3eTlKNjFNQ3BKa21MdlRXWFE9;MTgzNDIxMUAzMjMxMmUzMTJlMzQzMWo5S0l0TFBqR01TMVREOHdqUStSM0pmNWhmVDFIV2Y5ZUVQR2ZBbFBLbkk9;NRAiBiAaIQQuGjN/V0d+XU9Ad1RHQmFMYVF2R2BJelRzcV9DYUwgOX1dQl9gSXpRc0VlWX9bdnJVQGI=;MTgzNDIxM0AzMjMxMmUzMTJlMzQzMVBWVE16TSt6OWNFRVRrK2xtUHNSY3NBQnpUcnRLMXhYZnlNYXRpaDdHMUU9;MTgzNDIxNEAzMjMxMmUzMTJlMzQzMUttMWhueVpZTlZocFFqeVREbnhmYkVqQVQ1VEJSY0dtazhEWjhaNlBMZkE9;Mgo+DSMBMAY9C3t2VFhhQlVFfVpdX2ZWfFN0RnNcdV11flBHcC0sT3RfQF5jTXxWdkNjWXpYd3RdRA==;MTgzNDIxNkAzMjMxMmUzMTJlMzQzMUN1M0NldnVTMUw2K0dXaHlFR3ZDVzgwbERvd1dua2M3aDg0ME04aHpkdW89;MTgzNDIxN0AzMjMxMmUzMTJlMzQzMUo2YmQwSVowTmtZMmZpVUVRRUNnZUs0Y0RBRUFqeU5NVGFXMitrUGtVdTg9;MTgzNDIxOEAzMjMxMmUzMTJlMzQzMVBWVE16TSt6OWNFRVRrK2xtUHNSY3NBQnpUcnRLMXhYZnlNYXRpaDdHMUU9"
);

export default function App({ Component, pageProps }: AppProps) {
  return <Component {...pageProps} />
}
