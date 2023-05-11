import Image from "next/image";
import { Inter } from "next/font/google";
import DependencyMappingLayout from "./dependency-mapping";
const inter = Inter({ subsets: ["latin"] });

export default function Home() {
  return (
    <>
      <DependencyMappingLayout />
    </>
  );
}
