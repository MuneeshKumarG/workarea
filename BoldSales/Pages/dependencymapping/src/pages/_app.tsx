import "@/styles/globals.css";
import type { AppProps } from "next/app";
import { registerLicense } from "@syncfusion/ej2-base";
import "../../node_modules/@syncfusion/ej2-buttons/styles/tailwind.css";
import "../../node_modules/@syncfusion/ej2-base/styles/tailwind.css";
import "../../node_modules/@syncfusion/ej2-inputs/styles/tailwind.css";
import "../../node_modules/@syncfusion/ej2-react-dropdowns/styles/tailwind.css";

registerLicense(
  "Mgo+DSMBaFt+QHJqUU1lQ1NBaV1CX2BZe1l1QGlcfk4BCV5EYF5SRHNeQ11lTHpRcEJnXH8=;Mgo+DSMBPh8sVXJ1S0R+WFpBaV1HQmFJfFBmQGlaelR1dUU3HVdTRHRcQlhiSH5Vc0FhX3hYdnQ=;ORg4AjUWIQA/Gnt2VFhiQlVPc0BAWHxLflF1VWZTfFt6cVRWESFaRnZdQV1mSH9TcEBnW3lZeHRV;MjAwNzkwOUAzMjMxMmUzMjJlMzRieDdGMkkzZ2xBR0VCQ0VWbnJSTlBNcnpGV2V3anVjL3N2b1BTN0M4VjA4PQ==;MjAwNzkxMEAzMjMxMmUzMjJlMzRkTHUyd2FsWnNuN0ZRaXZuVUZac3pYTHgvZW1pZ0pNUFoza1REcWJlMW5NPQ==;NRAiBiAaIQQuGjN/V0d+Xk9AfVpdX2ZWfFN0RnNcdV11flBHcC0sT3RfQF5jTH9SdkNmXX1YeHBQRg==;MjAwNzkxMkAzMjMxMmUzMjJlMzRPa2JvTlFzajNoRmxXc3hVRjMwUmFBaGVNSWt6SFVFRkd1bnduUHM3ck9BPQ==;MjAwNzkxM0AzMjMxMmUzMjJlMzRZRy9ZOGtQYVlOcGdHL2FiVW9uZ2FBWjFsZURrSExPeXFGOFpNaUdNRGQ0PQ==;Mgo+DSMBMAY9C3t2VFhiQlVPc0BAWHxLflF1VWZTfFt6cVRWESFaRnZdQV1mSH9TcEBnW3lXcXVV;MjAwNzkxNUAzMjMxMmUzMjJlMzRDOHV2bHJYbGJSeGV6U05vVG40aGpCVmFiMm9WSzZrcnV2N0tyRUE5aHJZPQ==;MjAwNzkxNkAzMjMxMmUzMjJlMzRITjNKM2NaZVZ2OWhWSkZxc3pYTHZtNlFiMzY1T0h3TlVVYStzMFYyK3dJPQ==;MjAwNzkxN0AzMjMxMmUzMjJlMzRPa2JvTlFzajNoRmxXc3hVRjMwUmFBaGVNSWt6SFVFRkd1bnduUHM3ck9BPQ=="
);

export default function App({ Component, pageProps }: AppProps) {
  return <Component {...pageProps} />;
}
