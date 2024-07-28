import { Navigate, RouteObject, createBrowserRouter } from "react-router-dom";
import App from "../layout/App";
import HomePage from "../../features/home/HomePage";
import ActivityDashboard from "../../features/activities/dashboard/ActivityDashboard";
import ActivityForm from "../../features/activities/activityForm/ActivityForm";
import ActivityDetails from "../../features/activities/details/ActivityDetails";
import TestErrors from "../../features/Errors/TestError";
import NotFound from "../../features/Errors/NotFound";
import ServerError from "../../features/Errors/ServerError";
import LoginForm from "../../features/users/LoginForm";
import ProfilePage from "../../features/profiles/ProfilePage";
import ProfileChat from "../../features/profiles/ProfileChat";
import ActivityDetailedChat from "../../features/activities/details/ActivityDetailedChat";
import EmojiPickerComponent from "../../features/testComment/EmojiPicker";
import Payment from "../../features/Payments/Payment";
import PaymentResultPage from "../../features/Payments/PaymentResultPage";

export const routes: RouteObject[] = [
  {
    path: "/",
    element: <App />,
    children: [
      { path: "", element: <HomePage /> },
      { path: "/activities", element: <ActivityDashboard /> },
      { path: "/activities/:id", element: <ActivityDetails /> },
      { path: "/createActivity", element: <ActivityForm key="create" /> },
      { path: "/editActivity/:id", element: <ActivityForm key="manage" /> },
      { path: "/profiles/:username", element: <ProfilePage /> },
      { path: "/login", element: <LoginForm /> },
      { path: "/errors", element: <TestErrors /> },
      { path: "/not-found", element: <NotFound /> },
      { path: "/server-error", element: <ServerError /> },
      { path: "/profileChat", element: <ProfileChat /> },
      { path: "/emoji", element: <EmojiPickerComponent /> },

      { path: "/payment", element: <Payment /> },
      { path: "/PaymentResult/:username", element: <PaymentResultPage /> },
      {
        path: "/activityDetailedchat",
        element: <ActivityDetailedChat activityId="21" />,
      },
      { path: "*", element: <Navigate replace to="/not-found" /> },
    ],
  },
];

export const router = createBrowserRouter(routes);
