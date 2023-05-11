import "@/styles/globals.css";
import type { AppProps } from "next/app";
import { registerLicense } from "@syncfusion/ej2-base";
import "../../node_modules/@syncfusion/ej2-buttons/styles/tailwind.css";
import "../../node_modules/@syncfusion/ej2-base/styles/tailwind.css";
import "../../node_modules/@syncfusion/ej2-inputs/styles/tailwind.css";
import "../../node_modules/@syncfusion/ej2-react-dropdowns/styles/tailwind.css";

registerLicense(
  "Mgo+DSMBaFt+QHJqUU1lQ1NBaV1CX2BZe1l1QGlcfk4BCV5EYF5SRHNeQ1xlTXhQdUxhX3o=;Mgo+DSMBPh8sVXJ1S0R+WFpBaV1HQmFJfFBmQGlaelR1dUU3HVdTRHRcQlhiSH9VckNgWnZaeXE=;ORg4AjUWIQA/Gnt2VFhiQlVPc0BAWHxLflF1VWZTfFt6cVRWESFaRnZdQV1mSH9ScEFlWntedXNQ;MjAxNDMxNkAzMjMxMmUzMjJlMzRieDdGMkkzZ2xBR0VCQ0VWbnJSTlBNcnpGV2V3anVjL3N2b1BTN0M4VjA4PQ==;MjAxNDMxN0AzMjMxMmUzMjJlMzRkTHUyd2FsWnNuN0ZRaXZuVUZac3pYTHgvZW1pZ0pNUFoza1REcWJlMW5NPQ==;NRAiBiAaIQQuGjN/V0d+Xk9AfVpdX2ZWfFN0RnNcdV11flBHcC0sT3RfQF5jTH9Sd0NnX3xacXFcQw==;MjAxNDMxOUAzMjMxMmUzMjJlMzRPa2JvTlFzajNoRmxXc3hVRjMwUmFBaGVNSWt6SFVFRkd1bnduUHM3ck9BPQ==;MjAxNDMyMEAzMjMxMmUzMjJlMzRZRy9ZOGtQYVlOcGdHL2FiVW9uZ2FBWjFsZURrSExPeXFGOFpNaUdNRGQ0PQ==;Mgo+DSMBMAY9C3t2VFhiQlVPc0BAWHxLflF1VWZTfFt6cVRWESFaRnZdQV1mSH9ScEFlWntccnVQ;MjAxNDMyMkAzMjMxMmUzMjJlMzRDOHV2bHJYbGJSeGV6U05vVG40aGpCVmFiMm9WSzZrcnV2N0tyRUE5aHJZPQ==;MjAxNDMyM0AzMjMxMmUzMjJlMzRITjNKM2NaZVZ2OWhWSkZxc3pYTHZtNlFiMzY1T0h3TlVVYStzMFYyK3dJPQ==;MjAxNDMyNEAzMjMxMmUzMjJlMzRPa2JvTlFzajNoRmxXc3hVRjMwUmFBaGVNSWt6SFVFRkd1bnduUHM3ck9BPQ=="
);

export default function App({ Component, pageProps }: AppProps) {
  return <Component {...pageProps} />;
}
